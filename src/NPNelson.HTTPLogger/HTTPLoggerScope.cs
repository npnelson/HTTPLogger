using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Newtonsoft.Json;
using NPNelson.HTTPLogger.Abstractions;

namespace NPNelson.HTTPLogger
{

    public class HTTPLoggerScope
    {
        private readonly string _name;
        private readonly object _state;

        public HTTPLoggerScope(string name, object state)
        {
            _name = name;
            _state = state;
        }

        public ActivityContext Context { get; set; }

        public HTTPLoggerScope Parent { get; set; }

        public ScopeNode Node { get; set; }


        private static AsyncLocal<HTTPLoggerScope> _value = new AsyncLocal<HTTPLoggerScope>();
        public static HTTPLoggerScope Current
        {
            set
            {
                _value.Value = value;
            }
            get
            {
                return _value.Value;
            }
        }

        public static IDisposable Push(HTTPLoggerScope scope,ILogSource logSource)
        {
            if (scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }



            var temp = Current;
            Current = scope;
            Current.Parent = temp;

            Current.Node = new ScopeNode()
            {
                StartTime = DateTimeOffset.UtcNow,
                State = Current._state,
                Name = Current._name
            };

            if (Current.Parent != null)
            {
                Current.Node.Parent = Current.Parent.Node;
                Current.Parent.Node.Children.Add(Current.Node);
            }
            else
            {
                Current.Context.Root = Current.Node;


                //  store.AddActivity(Current.Context);
            }




            return new DisposableAction(() =>
            {
                Current.Node.EndTime = DateTimeOffset.UtcNow;
                if (Current.Parent == null)
                {
                    //we are at the root, now get all the messages for this request
                    var childNodes = Current.Node.Children.Traverse(x => x.Children);
                   
                    var messages = Current.Node.Messages.Union(childNodes.SelectMany(x=>x.Messages)).OrderBy(x=>x.Time);
                   // var httpInfoJson = JsonConvert.SerializeObject(Current.Context.HttpInfo);
                    logSource.WriteLog(Current.Context.HttpInfo, messages);             
                }
                Current = Current.Parent;
            });

        }

        private class DisposableAction : IDisposable
        {
            private Action _action;

            public DisposableAction(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                if (_action != null)
                {
                    _action.Invoke();
                    _action = null;
                }
            }
        }
    }

    public static class TraverseExtension
    {
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> fnRecurse)
        {
            foreach (T item in source)
            {
                yield return item;
                IEnumerable<T> seqRecurse = fnRecurse(item);
                if (seqRecurse != null)
                {
                    foreach (T itemRecurse in Traverse(seqRecurse, fnRecurse))
                    {
                        yield return itemRecurse;
                    }
                }
            }
        }
    }
}



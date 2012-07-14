using System;
namespace Webflow.Operations
{
    public abstract class OperationBase
    {
        public string Name { get; set; }
        public string Parameter { get; set; }
        public Action<object> CallbackAction { get; set; }
        public abstract void Execute(WebflowBase webflow);

        public OperationBase()
        {
            this.Name = "未命名";
        }

        public OperationBase(string param)
            : this()
        {
            this.Parameter = param;
        }

        public OperationBase(string param, Action<object> callback)
            : this(param)
        {
            this.CallbackAction = callback;
        }

        protected void InvokeCallback(object param)
        {
            if (this.CallbackAction != null)
            {
                this.CallbackAction.Invoke(param);
            }
        }
    }
}

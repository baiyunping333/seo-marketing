using System;
namespace Webflow.Operations
{
    public abstract class OperationBase
    {
        public string Name { get; set; }
        public OperationStatus Status { get; set; }
        public Action<object> CallbackAction { get; set; }
        public abstract void Execute(WebflowBase webflow);

        public OperationBase()
        {
            this.Name = "未命名";
            this.Status = OperationStatus.NotStarted;
        }

        public OperationBase(string name)
        {
            this.Name = name;
            this.Status = OperationStatus.NotStarted;
        }

        public OperationBase(string name, Action<object> callback)
            : this(name)
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

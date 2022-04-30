using UnityEngine;

namespace UI
{
    public abstract class AbstractMenu : MonoBehaviour
    {
        private IMenuContext _menuContext;

        public AbstractMenu Parent { get; private set; }

        public bool IsShown { get; protected set; }

        protected abstract void OnShow();
        protected abstract void OnClose();

        public void SetMenuContext(IMenuContext context)
        {
            _menuContext = context;
        }

        public void Show(AbstractMenu parent = null)
        {
            _menuContext.SetActiveMenu(this);

            Parent = parent;
            IsShown = true;
            OnShow();
        }

        public void Close()
        {
            OnClose();
            IsShown = false;
            _menuContext.SetActiveMenu(Parent);

            if (Parent != null)
            {
                Parent.OnChildClosed(this);
                Parent = null;
            }
        }

        public virtual void OnChildClosed(AbstractMenu child)
        {
        }

        public abstract void OnMenuKeyAction(MenuKeyAction action);
    }
}
namespace Player
{
    public class PlayerBindings
    {
        private InputBindings _inputBindings;

        public InputBindings GetBindings()
        {
            if (_inputBindings == null)
            {
                InitializeBindings();
            }

            return _inputBindings;
        }

        private void InitializeBindings()
        {
            _inputBindings = new InputBindings();
            _inputBindings.UI.Enable();
            _inputBindings.Player.Enable();
        }
    }
}
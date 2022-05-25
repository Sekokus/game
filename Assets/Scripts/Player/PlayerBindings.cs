namespace Sekokus.Player
{
    public class PlayerBindings
    {
        private InputBindings _inputBindings;

        public InputBindings GetBindings()
        {
            return _inputBindings ??= new InputBindings();
        }
    }
}
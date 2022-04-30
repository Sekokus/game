public class PlayerBindingsProxy : Singleton<PlayerBindingsProxy>
{
    private InputBindings _inputBindings;

    public InputBindings InputBindings => _inputBindings ??= new InputBindings();
}
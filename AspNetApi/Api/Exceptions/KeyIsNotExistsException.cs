namespace Api.Exceptions;

public class KeyIsNotExistsException : Exception {
	public KeyIsNotExistsException() : this("The key is not exists") { }
	public KeyIsNotExistsException(string message) : base(message) { }
}

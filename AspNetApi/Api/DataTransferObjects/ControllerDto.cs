namespace Api.DataTransferObjects;

public class ControllerDto {
	public string ControllerName { get; set; } = null!;

	public ControllerDto() : this(string.Empty) { }

	public ControllerDto(string controllerName) {
		ControllerName = controllerName;
	}
}
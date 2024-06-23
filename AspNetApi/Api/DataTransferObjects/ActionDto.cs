namespace Api.DataTransferObjects;

public class ActionDto : ControllerDto {
	public string ActionName { get; set; } = null!;

	public ActionDto() : this(string.Empty, string.Empty) { }

	public ActionDto(string controllerName, string actionName) : base(controllerName) {
		ActionName = actionName;
	}
}

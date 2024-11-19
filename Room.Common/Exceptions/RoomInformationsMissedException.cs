namespace Room.Common.Exceptions;
public class RoomInformationMissedException : Exception {
    public RoomInformationMissedException(string field) : base($"This field {field} must not be empty or null."){}
}
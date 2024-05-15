namespace ChatRoom.Application.Models {
    public class RabbitMessageDTO {

        public required string UserName { get; set; }
        public required string RoomName { get; set; }
        public required string Message { get; set; }

    }
}

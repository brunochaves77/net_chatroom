namespace ChatRoom.Application.Models.Responses {
    public class GenericResponse<Response> {

        public string Status { get; set; }
        public string? Message { get; set; }
        public Response? Data { get; set; }

        public GenericResponse(string status, string? message, Response? data) { 
            Status = status;
            Message = message;
            Data = data;
        }


    }
}

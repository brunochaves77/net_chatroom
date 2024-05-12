using ChatRoom.Application.Models.Responses;

namespace ChatRoom.Application.Factories {
    public static class GenericResponseFactory {

        public static GenericResponse<Response> Success<Response>(string message, Response? result) => new GenericResponse<Response>("success", message, result);
        public static GenericResponse<Response> Alert<Response>(string message, Response? result) => new GenericResponse<Response>("alert", message, result);
        public static GenericResponse<Response> Error<Response>(string message, Response? result) => new GenericResponse<Response>("error", message, result);

    }
}

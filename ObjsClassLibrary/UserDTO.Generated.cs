using ObjsClassLibrary.DTO;

namespace ObjsClassLibrary.DO
{
    internal static class ClassTest
    {
        internal static UserDTO MapToUserDTO(this UserDO obj)
        {
            UserDTO dest = new UserDTO();

            dest.UserName = obj.UserName;

            return dest;
        }
    }
}

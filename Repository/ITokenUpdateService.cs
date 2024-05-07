using Models.DtoModels;

namespace Repository
{
    public interface ITokenUpdateService
    {
        event TokenUpdateService.TokenHandlerServiceEventHandler tokenUpdated;

        void RaiseEvent(UserDTO cookie);
    }
}
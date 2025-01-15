using Diary.BLL.Services.Abstract;

namespace Diary.BLL.Services.Account
{
    public class UserIdStorage : IUserIdGetter, IUserIdSetter
    {
        private string _id;

        public string CurrentUserId { get => _id; }

        public string GetCurrentUserIdStrict()
        {
            return _id;
        }

        public void SetUserId(string userId)
        {
            _id = userId;
        }
    }
}

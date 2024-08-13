using System.Collections;

namespace Core
{
    public interface IService
    {
        IEnumerator Init();
        void Deinit();
    }
}
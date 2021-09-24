using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Internal;

namespace Ege.Check.App.Web.Common.ReCapture
{
    public interface IRecaptchaService
    {
        Task<RecaptchaResponse> Validate(string token);
    }
}

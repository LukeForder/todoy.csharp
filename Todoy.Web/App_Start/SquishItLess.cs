[assembly: WebActivator.PreApplicationStartMethod(typeof(Todoy.Web.App_Start.SquishItLess), "Start")]

namespace Todoy.Web.App_Start
{
    using SquishIt.Framework;
    using SquishIt.Less;

    public class SquishItLess
    {
        public static void Start()
        {
            Bundle.RegisterStylePreprocessor(new LessPreprocessor());
        }
    }
}
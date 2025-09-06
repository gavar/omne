namespace OMNE.Web.Layouts;

public class Layout<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T> : LayoutView where T : LayoutComponentBase
{
    public Layout() { Layout = typeof(T); }
}

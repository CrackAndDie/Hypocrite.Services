namespace Hypocrite.Core.Utils
{
    //
    // Summary:
    //     Specifies the display state of an element.
    public enum Visibility : byte
    {
        //
        // Summary:
        //     Display the element.
        Visible,
        //
        // Summary:
        //     Do not display the element, but reserve space for the element in layout.
        Hidden,
        //
        // Summary:
        //     Do not display the element, and do not reserve space for it in layout.
        Collapsed
    }
}

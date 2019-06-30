using AngleSharp.Dom;
using AngleSharp.Dom.Html;

namespace SunEngine.Core.Utils.TextProcess.PreviewsAnsSubTitle
{
    public static class MakePreview
    {
        public static string MakePreviewWithFirstImage(IHtmlDocument doc, int previewLength)
        {
            int currentSize = 0;
            var endText = (IText) FindTextNodePlus(doc.Body, ref currentSize, previewLength);
            if (endText != null)
                ClearNext(endText);

            var img1 = FindFirstBigImage(doc);
            if (img1 != null)
                ClearNext(img1);

            var iframe = FindFirstIFrame(doc);
            if (iframe != null)
                ClearNext(iframe);

            return doc.Body.InnerHtml;
        }

        public static string MakePreviewPlainText(IHtmlDocument doc, int previewLength)
        {
            if(doc.TextContent.Length < previewLength)
                return doc.TextContent;
            else
                return doc.TextContent.Substring(0, previewLength) + "...";
        }

        public static string MakePreviewTextWithOutImages(IHtmlDocument doc, int previewLength)
        {
            int currentSize = 0;
            var endText = (IText) FindTextNodePlus(doc.Body, ref currentSize, previewLength);
            if (endText != null)
                ClearNext(endText);

            foreach (var element in doc.QuerySelectorAll("iframe"))
                element.Remove();
            
            foreach (var htmlImageElement in doc.Images)
                htmlImageElement.Remove();
            
            return doc.Body.InnerHtml;
        }
        

        private static IElement FindFirstIFrame(IHtmlDocument doc)
        {
            return doc.QuerySelector("iframe");
        }

        private static INode FindTextNodePlus(INode ell, ref int currentSize, int maxSize)
        {
            if (ell.NodeType == NodeType.Text)
            {
                currentSize += ell.TextContent.Length;
                if (currentSize >= maxSize)
                {
                    ell.TextContent = ell.TextContent.Substring(0, ell.TextContent.Length - currentSize + maxSize) +
                                      "...";
                    return ell;
                }

                return null;
            }

            foreach (var el in ell.ChildNodes)
            {
                var ret = FindTextNodePlus(el, ref currentSize, maxSize);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        private static IHtmlImageElement FindFirstBigImage(IHtmlDocument doc)
        {
            foreach (var imgEl in doc.Images)
            {
                if (imgEl.Source.ToLower().Contains("emoticons")) // TODO определение смайликов
                    continue;
                
                return imgEl;
            }

            return null;
        }

        private static void ClearNext(INode ell)
        {
            if (ell == null)
                return;

            var cell = ell.NextSibling;
            while (cell != null)
            {
                var next = cell.NextSibling;

                if (cell is IElement el)
                    el.Remove();
                else if (cell is IText text)
                    text.Remove();

                cell = next;
            }

            ClearNext(ell.ParentElement);
        }

        static INode GetLastTextNode(INode node)
        {
            if (node.LastChild == null)
                return null;
            if (node.LastChild.NodeType == NodeType.Text)
                return node;

            return GetLastTextNode(node.LastChild);
        }
    }
}

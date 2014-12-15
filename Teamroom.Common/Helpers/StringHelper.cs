using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HobbyClue.Common.Models;
using Microsoft.Security.Application;

namespace HobbyClue.Common.Helpers
{
    public static class StringHelper
    {
        public const double KiloBytes = 1024;
        public const double MegaBytes = 1048576;
        static readonly Regex AmpersandCharacterRegex = new Regex(@"&amp;", RegexOptions.Compiled);
        public static readonly string Ellipses = "...";
        static readonly Regex EncodedAmpersandRegex = new Regex(@"&(#38|amp);", RegexOptions.Compiled);
        static readonly Regex EncodedNewLinesRegex = new Regex(@"(&#13;)?&#10;", RegexOptions.Compiled);
        static readonly Regex EncodedSemicolonRegex = new Regex(@"&#59;", RegexOptions.Compiled);
        static readonly Regex EncodedTabsRegex = new Regex(@"&#0*9;", RegexOptions.Compiled);
        static readonly Regex EncodedTagsRegex = new Regex(@"&lt;([^<>]*?)gt;", RegexOptions.Compiled | RegexOptions.Singleline);
        static readonly Regex SingleEncodedTagsRegex = new Regex(@"(?<!&lt;)&lt;(?!&lt;)[^<>]*?(?<!&gt;)&gt;(?!&gt;)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        static readonly Regex EncodedWBR = new Regex(@"&#60;wbr&#62;", RegexOptions.Compiled);
        static readonly Regex GreaterThanSignCharacterRegex = new Regex(@"&gt;", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static readonly Regex HtmlBodyContentRegex = new Regex(@"(?<=<body.*?>).*(?=</body\s*?>)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly string HtmlBreak = "<br />";
        public static readonly string HtmlSpace = "&nbsp;";
        public static readonly string HtmlTabSpaces = "&nbsp;&nbsp;&nbsp;&nbsp;";
        public static readonly string HtmlWordBreak = "<wbr></wbr>";
        public static readonly string HtmlHighlightOpenTag = "<span class='HighlightItem'>";
        public static readonly string HtmlHighlightCloseTag = "</span>";
        static readonly Regex LessThanSignCharacterRegex = new Regex(@"&lt;", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly string LineBreak = "\r\n";
        static readonly Regex MultipleSpacesRegex = new Regex(@"  +", RegexOptions.Compiled);
        static readonly Regex LeadingSpaceRegex = new Regex(@"^ +", RegexOptions.Compiled);
        static readonly Regex TrailingSpaceRegex = new Regex(@" +$", RegexOptions.Compiled);

        static readonly Regex NewLinesRegex = new Regex(@"(\r)?\n", RegexOptions.Compiled);
        static readonly Regex NonBreakingSpaceCharacterRegex = new Regex(@"&nbsp;", RegexOptions.Compiled);
        static readonly Regex NonEncodedBR = new Regex(@"\<br\s*?\/?\>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly string NonHtmlSpaces = "   ";

        static readonly Regex QuotationMarkCharacterRegex = new Regex(@"&quot;|&ldquo;|&rdquo;", RegexOptions.Compiled);
        static readonly Regex SingleQuoteMarkCharacterRegex = new Regex(@"&lsquo;|&rsquo;", RegexOptions.Compiled);
        static readonly Regex SafeClassRegex;
        static readonly Regex SafeStyleRegex;
        static readonly Regex SafeTagRegex = BuildSafeTagRegex();
        static readonly Regex ScriptTagRegex = new Regex(@"<script\b[^>]*>.*</script>\r?\n?", RegexOptions.Compiled);
        static string[] UnsafeStyleRegexBlackList;

        static readonly Regex ImageFinder = new Regex(@"(gif|jpeg|jpg|bmp|png)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static readonly Regex FullStyleStripperRegex = new Regex(@"(&lt;STYLE&gt;).*(&lt;/STYLE&gt;)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        static readonly Regex SafePrettifyClassesRegex = new Regex(@"(?<=class=)[""']x_(pretty|lang|str|kwd|com|typ|lit|opn|clo|pun|pln|sig|tag|atn|atv|dec|fun|var).*?[""']+?", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        //Detects orphaned <li> elements that are not wrapped with a preceeding <ul> element. 
        //Regex Walk -> (no proceeding <ul> negative look behind)<li>(*any chars)<li>(no following </ul> negative look ahead)
        static readonly Regex OrphanedListItemsRegex = new Regex(@"(?<!</ul>|<li>|</ol>)<li>.*</li>(?!</ul>|<li>|</ol>)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        
        static StringHelper()
        {
            var blockedStyles = new[]
                {
                    "overflow",
                    "position",
                    "z-index",
                    "top",
                    "left",
                    "visibility"
                };

            SafeStyleRegex = BuildSafeStyleRegex(blockedStyles);
            SafeClassRegex = BuildSafeClassRegex();
        }

        static Regex BuildSafeStyleRegex(string[] blockedStyles)
        {
            string styleRegex = @"{0}\s*?:\s*[^;""]*";
            UnsafeStyleRegexBlackList = new string[blockedStyles.Length];
            for (int i = 0; i < blockedStyles.Length; i++)
            {
                UnsafeStyleRegexBlackList[i] = String.Format(styleRegex, blockedStyles[i]);
            }

            string regEx = String.Format("((?<=<.*?style\\s*?=\\s*?\".*?)({0});?(?=.*?\".*?>))",
                                         String.Join("|", UnsafeStyleRegexBlackList));
            return new Regex(regEx, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }
        
        static Regex BuildSafeClassRegex()
        {
            const string classRegexRemove = "class\\s*?=\\s*?\".*?\"";
            string regEx = String.Format("((?<=<.*?)({0});?(?=.*?>))", classRegexRemove);
            return new Regex(regEx, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        /// <summary>
        /// Gets the safe Html for the input Html retaining the data-chameleon-template attribute.
        /// Sanitizer strips offs any other data attribute along with other unsafe html.
        /// </summary>
        /// <param name="html">input html string</param>
        /// <returns>sanitized html string</returns>
        public static string GetSafeHtmlRetainingChameleonTemplate(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            string safeHtml = html;
        
            var chameleonPatternRegex = new Regex(@"<div (?<tag>data-chameleon-template)=['""]?(?<template>[^'"" ]+)['""]?.*((/>)|(></div>))", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            safeHtml = chameleonPatternRegex.Replace(safeHtml, m =>
                                                              {
                                                                  var templateTag = m.Groups["tag"].Value;
                                                                  var templateName = m.Groups["template"].Value;
                                                                  return m.Value.Replace(templateTag, "class").Replace(templateName, String.Format("ctemplate_{0}", templateName));
                                                              });
            safeHtml = Sanitizer.GetSafeHtml(safeHtml);

            var chameleonPatternMaskRegex =
                new Regex(@"<div (?<template>class=[""']ctemplate_()).*?[""']+?.*((/>)|(></div>))",
                          RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

            safeHtml = chameleonPatternMaskRegex.Replace(safeHtml, m =>
            {
                var template = m.Groups["template"].Value;
                return m.Value.Replace(template, @"data-chameleon-template=""");
            });

            return safeHtml;
        }

        public static string GetSafeHTMLWithRemovedInlinesStylesThatAreBlackListed(string styleUnsafeHtml)
        {
            string styleSafeHtml = SafeStyleRegex.Replace(styleUnsafeHtml, string.Empty);
            return styleSafeHtml;
        }

        public static string GetSafeHTMLWithRemovedInClass(string styleUnsafeHtml)
        {
            string styleSafeHtml = SafeClassRegex.Replace(styleUnsafeHtml, string.Empty);
            return styleSafeHtml;
        }

        static Regex BuildSafeTagRegex()
        {
            string regEx = String.Format(@"(\&lt;(\/)?({0})\b(.|\n)*?\&gt);", String.Join("|", Enum.GetNames(typeof(SafeHtmlTags))));
            return new Regex(regEx, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static string GetSafeHtmlBodyOnly(string unsafeHtml)
        {
            Match m = HtmlBodyContentRegex.Match(unsafeHtml);
            return m.Value;
        }

        public static string GetSafeHtml(string unsafeHtml)
        {
            if (String.IsNullOrEmpty(unsafeHtml)) return unsafeHtml;
            bool addclassPrettyprint = false;
            
            string safeHtml = GetSafeHTMLWithRemovedScriptTags(unsafeHtml);
        	safeHtml = RemoveHtmlComments(safeHtml);
            safeHtml = Encode(safeHtml);
            safeHtml = SafeTagRegex.Replace(safeHtml, DecodeMatch);
            safeHtml = GetSafeHTMLWithRemovedInlinesStylesThatAreBlackListed(safeHtml);
            safeHtml = WrapOrphanedListItems(safeHtml);
            if (safeHtml.Contains("<pre"))
            {
                safeHtml = safeHtml.Replace("<span class=", "<span langcode=");
                safeHtml = safeHtml.Replace("<pre class=", "<pre langcode=");
                addclassPrettyprint = true;
            }
            safeHtml = GetSafeHTMLWithRemovedInClass(safeHtml);
            if (addclassPrettyprint)
            {
                safeHtml = safeHtml.Replace("<pre langcode=", "<pre class=");
                safeHtml = safeHtml.Replace("<span langcode=", "<span class=");
            }
            safeHtml = Sanitizer.GetSafeHtmlFragment(safeHtml);
            safeHtml = GetSafeHtmlWithEscapedCharacters(safeHtml);
            safeHtml = SafePrettifyClassesRegex.Replace(safeHtml, m => m.Value.Replace("x_", string.Empty));

            safeHtml = RemoveLinksWithJavascript(safeHtml);

            safeHtml = safeHtml.Trim();
            return safeHtml;
        }

        private static string RemoveLinksWithJavascript(string safeHtml)
        {
            string pattern = @"([<]a[\s]*href=[^>]*javascript&colon;[^>]*[>])([^<]*)([<]/a[>])";
            var removeLink = Regex.Replace(safeHtml, pattern, "$2");


            return removeLink;
        }

        public static string RemoveHtmlComments(string text)
		{
			var regEx = new Regex(@"\<![ \r\n\t]*(--([^\-]|[\r\n]|-[^\-])*--[ \r\n\t]*)\>", RegexOptions.IgnoreCase);
			text = regEx.Replace(text, String.Empty);
			return text;
		}

        static string GetSafeHtmlWithEscapedCharacters(string safeHtml)
        {
            safeHtml = EncodedAmpersandRegex.Replace(safeHtml, "&");
            safeHtml = EncodedSemicolonRegex.Replace(safeHtml, ";");
            return safeHtml;
        }

        static string DecodeMatch(Match m)
        {
            return Decode(m.ToString());
        }

        public static string ReplaceEncodedNewLinesWithNonHtmlSpaces(string sourceText)
        {
            return EncodedNewLinesRegex.Replace(sourceText, NonHtmlSpaces);
        }

        public static string ReplaceEncodedNewLinesWithSpaces(string sourceText)
        {
            return EncodedNewLinesRegex.Replace(sourceText, HtmlSpace);
        }

        public static string ReplaceEncodedNewLinesWithBreaks(string sourceText)
        {
            return EncodedNewLinesRegex.Replace(sourceText, HtmlBreak);
        }

        public static string ReplaceNewLinesWithBreaks(string sourceText)
        {
            return NewLinesRegex.Replace(sourceText, HtmlBreak);
        }

        public static string ReplaceEncodedBRWithHtml(string sourceText)
        {
            return EncodedWBR.Replace(sourceText, HtmlWordBreak);
        }

        public static string ReplaceNonEncodedBRWithLineBreak(string sourceText)
        {
            return NonEncodedBR.Replace(sourceText, LineBreak);
        }

        public static string RemoveNonEncodedBR(string sourceText)
        {
            return RemoveNonEncodedBR(sourceText, string.Empty);
        }

        public static string RemoveNonEncodedBR(string sourceText, string replacement)
        {
            return NonEncodedBR.Replace(sourceText, replacement);
        }

        public static string RemoveLineBreaks(string sourceText)
        {
            return NewLinesRegex.Replace(sourceText, string.Empty);
        }


        public static string TrimWithEllipses(string sourceText, int maxLength)
        {
            if (string.IsNullOrEmpty(sourceText)) return sourceText;
            if (sourceText.Length > maxLength)
                return sourceText.SubstringToWordEnd(maxLength - 3) + Ellipses;
            return sourceText;
        }

        static string ConvertHtmlTableToText(string sourceText)
        {
            sourceText = Regex.Replace(sourceText, @"(?<=t[able|body|d|h|r].*?>.*?)\s+?(?=<.?t[able|body|d|h|r])", "", RegexOptions.Compiled | RegexOptions.Multiline);
            sourceText = Regex.Replace(sourceText, "<table.*?>", "\r\n", RegexOptions.Compiled | RegexOptions.Multiline);
            sourceText = Regex.Replace(sourceText, "</t[h|d].*?>", " ", RegexOptions.Compiled | RegexOptions.Multiline);
            sourceText = Regex.Replace(sourceText, "</tr.*?>", "\r\n", RegexOptions.Compiled | RegexOptions.Multiline);

            return sourceText;
        }

        public static string RemoveHtmlTags(string sourceText)
        {
            return Regex.Replace(sourceText, @"<(.|\n)*?>", "", RegexOptions.Compiled | RegexOptions.Multiline);
        }

        public static string RemoveCarriageReturns(string sourceText)
        {
            return sourceText.Replace("\r\n", string.Empty);
        }

        public static string ReplaceCarriageReturns(string sourceText)
        {
            if (sourceText.IndexOf('\r') > -1)
            {
                if (sourceText.IndexOf("\r\n") > -1)
                    sourceText = string.Join("\n", sourceText.Split(new[] { "\r\n" }, StringSplitOptions.None));
                if (sourceText.IndexOf('\r') > -1)
                    sourceText = string.Join("\n", sourceText.Split('\r'));
            }
            return sourceText;
        }

        public static string GetInnerTextWithConvertedLineBreaks(string sourceText)
        {
            sourceText = RemoveCarriageReturns(sourceText);
            sourceText = ConvertHtmlTableToText(sourceText);
            sourceText = ReplaceNonEncodedBRWithLineBreak(sourceText);
            sourceText = RemoveHtmlTags(sourceText);
            sourceText = ReplaceSpecialHtmlCharacters(sourceText);
            return sourceText;
        }

        public static string GetInnerTextWithConvertedLineBreaksNoRemoveCRsRemoveBRNoRemoveEncodedHtml(string sourceText)
        {
            sourceText = ConvertHtmlTableToText(sourceText);
            sourceText = RemoveNonEncodedBR(sourceText);
            sourceText = Regex.Replace(sourceText, "<p>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            sourceText = Regex.Replace(sourceText, "</p>", LineBreak, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            sourceText = RemoveHtmlTags(sourceText);
            sourceText = ReplaceSpecialHtmlCharacters(sourceText);
            return sourceText;
        }

        public static string ReplacePWithDoubleBreak(string sourceText)
        {
            if (sourceText.ToLower().EndsWith("</p>"))
                sourceText = sourceText.Remove(sourceText.ToLower().LastIndexOf("</p>"));

            sourceText = Regex.Replace(sourceText, "<p>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            sourceText = Regex.Replace(sourceText, "</p>", "<br /><br />", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return sourceText;
        }

        public static string ReplacePWithLineBreak(string sourceText)
        {
            if (sourceText.ToLower().EndsWith("</p>"))
                sourceText = sourceText.Remove(sourceText.ToLower().LastIndexOf("</p>"));

            sourceText = Regex.Replace(sourceText, "<p>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            sourceText = Regex.Replace(sourceText, "</p>", @"\r\n\", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return sourceText;
        }

        public static string GetInnerText(string sourceText)
        {
            string retVal = Regex.Replace(sourceText, @"\s+|\t", " ", RegexOptions.Compiled | RegexOptions.Multiline);
            return Regex.Replace(retVal, @"<(.|\n)*?>|\n|\r", "", RegexOptions.Compiled | RegexOptions.Multiline);
        }

        /// <summary>
        /// Inserts WBR hints in words longer than the length specified by maxWordLength, and html encodes the sourceText.
        /// </summary>
        /// <param name="sourceText">The NON-HTML-ENCODED string to parse.</param>
        /// <param name="maxWordLength">The maximum number of characters to allow in each word</param>
        /// <returns></returns>
        public static string InsertWordBreaksAndHtmlEncode(string sourceText, int maxWordLength)
        {
            if (sourceText.Length < maxWordLength) return Encode(sourceText);

            string HtmlWordBreakToken = ".xXTKNXx.";

            while (sourceText.IndexOf(HtmlWordBreakToken) > -1) HtmlWordBreakToken += "x";
            string wordRegex = @"([^\s]{" + maxWordLength + @"})(?=[^\s])";
            Regex regex = new Regex(wordRegex, RegexOptions.Compiled);
            sourceText = regex.Replace(sourceText, String.Format("$0{0}", HtmlWordBreakToken));

            return Encode(sourceText).Replace(HtmlWordBreakToken, HtmlWordBreak);
        }

        static string Encode(string source)
        {
            return HttpUtility.HtmlEncode(source);
        }

        public static string Decode(string source)
        {
            return HttpUtility.HtmlDecode(source);
        }


        public static string PreserveEncodedWhitespaceForHtml(string sourceText)
        {
            sourceText = EncodedTabsRegex.Replace(sourceText, HtmlTabSpaces);
            sourceText = ReplaceEncodedNewLinesWithBreaks(sourceText);
            sourceText = LeadingSpaceRegex.Replace(sourceText, AllSpaceReplaceEvaluator);
            sourceText = TrailingSpaceRegex.Replace(sourceText, AllSpaceReplaceEvaluator);
            sourceText = MultipleSpacesRegex.Replace(sourceText, MultipleSpaceReplaceEvaluator);

            return sourceText;
        }

        static string AllSpaceReplaceEvaluator(Match match)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < match.Length; i++)
                sb.Append(HtmlSpace);

            return sb.ToString();
        }

        static string MultipleSpaceReplaceEvaluator(Match match)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" ");

            for (int i = 0; i < match.Length - 1; i++)
                sb.Append(HtmlSpace);

            return sb.ToString();
        }

        public static string FormatBytes(long sizeInBytes)
        {
            if (sizeInBytes < 0) sizeInBytes = 0;
            if (sizeInBytes < KiloBytes)
                return string.Format("{0} Bytes", sizeInBytes);

            string suffix;
            double convertedValue;

            if (sizeInBytes < MegaBytes)
            {
                suffix = "KB";
                convertedValue = sizeInBytes / KiloBytes;
            }
            else
            {
                suffix = "MB";
                convertedValue = sizeInBytes / MegaBytes;
            }

            return String.Format("{0:F1} {1}", convertedValue, suffix);
        }

        public static string RemoveEncodedTags(string unsafeHtml)
        {
            return EncodedTagsRegex.Replace(unsafeHtml, string.Empty);
        }

        public static string ReplaceSpecialHtmlCharacters(string html)
        {
            html = QuotationMarkCharacterRegex.Replace(html, "\"");
            html = SingleQuoteMarkCharacterRegex.Replace(html, "'");
            html = AmpersandCharacterRegex.Replace(html, "&");
            html = LessThanSignCharacterRegex.Replace(html, "<");
            html = GreaterThanSignCharacterRegex.Replace(html, ">");
            html = NonBreakingSpaceCharacterRegex.Replace(html, " ");
            return html;
        }

        public static string EncodeSpecialHtmlCharacters(string html)
        {
            html = html.Replace("&", "&amp;");
            html = html.Replace("<", "&lt;");
            html = html.Replace(">", "&gt;");
            html = html.Replace("\"", "&quot;");
            html = html.Replace("\'", "&apos;");
            return html;
        }

        public static string HexEncodeHtmlTags(string html)
        {
            html = LessThanSignCharacterRegex.Replace(html, "&#60;");
            html = GreaterThanSignCharacterRegex.Replace(html, "&#62;");
            return html;
        }

        public static string RemoveSingleEncodedHtmlTags(string html)
        {
            html = SingleEncodedTagsRegex.Replace(html, string.Empty);
            return html;
        }

        public static string TrimWithEllipsesAndHtmlEncode(string sourceText, int trimLength, int maxWordLength)
        {
            if (string.IsNullOrEmpty(sourceText)) return sourceText;

            string ending = string.Empty;
            if (sourceText.Length > trimLength)
            {
                sourceText = sourceText.Substring(0, trimLength - 3);
                ending = Ellipses;
            }

            return InsertWordBreaksAndHtmlEncode(sourceText, maxWordLength) + ending;
        }

        private static string GetSafeHTMLWithRemovedScriptTags(string html)
        {
            return ScriptTagRegex.Replace(html, string.Empty);
        }

        public static bool IsImageFile(string path)
        {
            return ImageFinder.IsMatch(path);
        }

        #region Nested type: SafeHtmlTags

        enum SafeHtmlTags
        {
            a,
            b,
            i,
            u,
            s,

            h1,
            h2,
            h3,
            h4,
            h5,
            h6,
            hr,
            p,
            br,
            nobr,
            wbr,
            pre,
            code,
            em,
            strong,

            blockquote,
            address,

            div,
            span,
            img,
            table,
            caption,
            tbody,
            tr,
            th,
            td,
            thead,
            colgroup,
            col,
            ul,
            ol,
            li,
            dl,
            dt,
            dd,

            font
        }

        #endregion

        public static string GetSafeHTMLWithRemovedStyles(string unsafeHtml)
        {
            return FullStyleStripperRegex.Replace(unsafeHtml, string.Empty);
        }

        public static string NormalizeText(string text)
        {
            text = text.Trim();
            if (text.IndexOf('\r') > -1)
            {
                if (text.IndexOf("\r\n") > -1)
                    text = string.Join("\n", text.Split(new[] { "\r\n" }, StringSplitOptions.None));
                if (text.IndexOf('\r') > -1)
                    text = string.Join("\n", text.Split('\r'));
            }
            return text;
        }

        public static string WrapOrphanedListItems(string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            return OrphanedListItemsRegex.Replace(html, (match) =>
            {
                var liItems = match.Value;
                return "<ul>" + liItems + "</ul>";
            });
        }

        public static string TruncateAfterWord(this string longString, int length, bool addEllipsis = false)
        {
            if (string.IsNullOrEmpty(longString) || longString.Length <= length) return longString;
            
            var spaceIndex = longString.LastIndexOf(" ", length, StringComparison.InvariantCulture);
            return longString.Substring(0, spaceIndex > 0 ? spaceIndex : length) + (addEllipsis ? " ..." : "");
        }

        public static string MakeUrlFriendly(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;

            str = str.Trim();
            str = str.ToLowerInvariant();
            str = RemoveEverythingExceptLettersAndNumbersAndSpaces(str);
            str = Regex.Replace(str, @"[\s]+", "-", RegexOptions.Compiled);
            str = HttpUtility.UrlEncode(str);
            return str;
        }

        public static string AlterSearchTermToAccomodateAspNetRequestValidationBug(string str)
        {
             str = str.Replace("<", "< ");
             str = str.Replace("&#", "& #");
            return str;
        }

        public static string RemoveEverythingExceptLettersAndNumbersAndSpaces(string str)
        {
            return Regex.Replace(str, @"[^a-zA-Z0-9\s]+", "", RegexOptions.Compiled);
        }

        public static string ChangeFirstLetterToUppercase(string text)
        {
            return string.IsNullOrEmpty(text) ? string.Empty : char.ToUpper(text[0]) + text.Substring(1);
        }

        public static string ChangeFirstLetterToLowercase(string text)
        {
            return string.IsNullOrEmpty(text) ? string.Empty : char.ToLower(text[0]) + text.Substring(1);
        }
    }
}

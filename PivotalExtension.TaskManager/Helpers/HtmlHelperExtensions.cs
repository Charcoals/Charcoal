using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PivotalExtension.TaskManager {
	public static class HtmlHelperExtensions {
		//static Regex UrlRegex = new Regex(@"http(s)?\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?", RegexOptions.Compiled);
		static Regex UrlRegex = new Regex(@"(?i)\b((?:[a-z][\w-]+:(?:/{1,3}|[a-z0-9%])|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’]))", RegexOptions.Compiled);

		public static string FormatLinksInPlace(this HtmlHelper helper, string input) {
			var decoded = input;
			var matches = UrlRegex.Matches(decoded);
			foreach (Match match in matches) {
				decoded = decoded.Replace(match.Value, string.Format("<a href='{0}'>{0}</a>", match.Value));
			}
			return decoded;
		}
	}
}
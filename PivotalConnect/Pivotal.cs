using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace PivotalConnect {
	public class Pivotal {
		public static Pivotal Instance = new Pivotal();

		const string PivotalUrl = @"https://www.pivotaltracker.com/services/v3/";
		private const int ProjectId = 424921;

		string token;

		public static List<Member> Members;

		public Pivotal() {
			SetToken();
			SetMembers();
		}

		//public List<Story> GetStories() {
		//  var storiesXDoc = GetPivotalResponse("projects/$PROJECTID/iterations/current", new Dictionary<string, string> { { "projectId", ProjectId.ToString() } });
		//  var storiesXObj = storiesXDoc.Descendants("story");

		//  var stories = storiesXObj.Select(s => new Story {
		//    Id = int.Parse(s.Descendants("id").First().Value),
		//    Type = (StoryType)Enum.Parse(typeof(StoryType), s.Descendants("story_type").First().Value, true),
		//    Name = s.Descendants("name").First().Value,
		//    Tasks = s.Descendants("task").Select(t => new Task {
		//      Id = int.Parse(t.Descendants("id").First().Value),
		//      Description = t.Descendants("description").First().Value,
		//      Complete = bool.Parse(t.Descendants("complete").First().Value)
		//    }).OrderBy(t => t.Complete).ToList()
		//  }).ToList();

		//  return stories;
		//}

		//public List<Task> GetTasks(int storyId) {
		//  var parameters = new Dictionary<string, string> {
		//    {"projectId", ProjectId.ToString()},
		//    {"storyId", storyId.ToString()}
		//  };
		//  var tasksXDoc = GetPivotalResponse("projects/$PROJECTID/stories/$STORYID", parameters);

		//  return tasksXDoc.Descendants("task").Select(task => new Task {
		//    Id = int.Parse(task.Descendants("id").First().Value),
		//    Description = task.Descendants("description").First().Value,
		//    Complete = bool.Parse(task.Descendants("complete").First().Value)
		//  }).ToList();
		//}

		XDocument GetPivotalResponse(string path, Dictionary<string, string> parameters) {
			var uri = PivotalUrl + path + "?token=" + token;
			uri = parameters.Aggregate(uri, (current, parameter)
				=> current.Replace("$" + parameter.Key.ToUpper(), parameter.Value));

			var req = WebRequest.Create(uri);
			var resp = req.GetResponse();
			var sr = new StreamReader(resp.GetResponseStream());
			return XDocument.Parse(sr.ReadToEnd().Trim());
		}

		void SetToken() {
			var req = WebRequest.Create(PivotalUrl + "tokens/active");
			req.Credentials = new NetworkCredential("v5core", "changeme");
			var resp = req.GetResponse();
			var sr = new StreamReader(resp.GetResponseStream());
			token = XDocument.Parse(sr.ReadToEnd().Trim()).Descendants("token").First().Descendants("guid").First().Value;
		}

		void SetMembers() {
			var membersXDoc = GetPivotalResponse("projects/$PROJECTID/memberships",
				new Dictionary<string, string> { { "projectId", ProjectId.ToString() } });

			Members = membersXDoc.Descendants("membership").Select(
				m => new Member {
					Id = int.Parse(m.Descendants("id").First().Value),
					Name = m.Descendants("person").First().Descendants("name").First().Value,
					Initials = m.Descendants("person").First().Descendants("initials").First().Value
				}).ToList();
		}
	}
}
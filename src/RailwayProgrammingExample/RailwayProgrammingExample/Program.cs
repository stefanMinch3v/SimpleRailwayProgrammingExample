namespace RailwayProgrammingExample
{
	using Common;
	using Models.ValueObjects;
	using System;
	using System.Linq;

	// operations:
	// - display
	// - search {id}
	// - create {name} {age} {membershipId}
	// - delete {id}
	// - promote {userId}
	public class Program
    {
        public static void Main()
        {
			var input = Console.ReadLine();

			while (!input.Contains("stop"))
			{
				try
				{
					var inputParsed = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					var operation = inputParsed[0];

					if (operation.Contains("display"))
					{
						GetAllUsers();
					}
					else if (operation.Contains("create"))
					{
						var name = inputParsed[1];
						var age = int.Parse(inputParsed[2]);
						var membershipId = int.Parse(inputParsed[3]);

						CreateUser(name, age, membershipId);
					}
					else if (operation.Contains("search"))
					{
						var searchId = int.Parse(inputParsed[1]);
						GetUser(searchId);
					}
					else if (operation.Contains("delete"))
					{
						var deleteId = int.Parse(inputParsed[1]);
						DeleteUser(deleteId);
					}
					else if (operation.Contains("promote"))
					{
						var userId = int.Parse(inputParsed[1]);
						PromoteUser(userId);
					}
					else
					{
						Console.WriteLine("Invalid operation.");
					}
				}
				catch
				{
					Console.WriteLine("Invalid inputs.");
				}

				input = Console.ReadLine();
			}
		}

		private static void PromoteUser(int id)
		{
			var message = Database.GetUser(id)
				.ToResult($"User with id {id} does not exist.")
				.Ensure(u => u.CanChangeMembership, "The user has already the highest membership.")
				.OnSuccess(u => Database.PromoteUser(id))
				.OnSuccess(u => Console.WriteLine("Sent email to user from some service."))
				.OnBoth(r => r.IsSuccess ? SuccessMsg() : ErrorMsg(r.ErrorMsg));

			Console.WriteLine(message);
		}

        private static void CreateUser(string userName, int age, int membershipId)
        {
			var userNameResult = UserName.Create(userName);
			var userAgeResult = UserAge.Create(age);
			var userMembership = UserMembership.Create(membershipId);

			var result = Result.Combine(userNameResult, userAgeResult, userMembership);
			if (result.IsFailure)
			{
				Console.WriteLine(result.ErrorMsg);
				Console.WriteLine("Try again.");
				return;
			}

			var createdId = Database.CreateUser(userNameResult.Value, userAgeResult.Value, userMembership.Value);

			Console.WriteLine($"Created with id: {createdId}");
		}

		private static void GetUser(int id)
		{
			var userOrNothing = Database.GetUser(id);
			if (userOrNothing.HasNoValue)
			{
				Console.WriteLine($"User with id:{id} does not exist.");
				return;
			}

			Console.WriteLine(userOrNothing.Value.ToString());
		}

        private static void GetAllUsers()
        {
			var users = Database.GetAllUsers();
			
			var reducedUsers = users.Where(u => u != null);
			if (!reducedUsers.Any())
			{
				Console.WriteLine("No users found.");
				return;
			}

			Console.WriteLine(string.Join("\n", users.Select(u => u.ToString())));
        }

        private static void DeleteUser(int id)
        {
			var result = Database.DeleteUser(id);
			if (result.IsFailure)
			{
				Console.WriteLine(result.ErrorMsg);
				return;
			}

			Console.WriteLine("Successfully removed.");
        }

		private static string SuccessMsg()
			=> "Logger: Log Success";

		private static string ErrorMsg(string errorMsg)
			=> $"Logger: Log Error: {errorMsg}";
    }
}

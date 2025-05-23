using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeesController : ControllerBase
	{
		/// <summary>
		/// Gets the list of all employees
		/// </summary>
		/// <return>The list of employees</return>
		[HttpGet]
		public IEnumerable<Employee> Get()
		{
			return Getemployees();
		}


		[HttpDelete("{Id}")]
		public void Delete(int id)
		{

		}



		[HttpGet("{id}", Name ="Get")]
		public Employee  Get(int id)
		{
			return Getemployees().FirstOrDefault(e => e.Id == id);
		}
		/// <summary>
		/// Create an Employee
		/// </summary>
		/// <remarks>
		/// POST api/Employee
		/// {
		///		"firstName":"Mike",
		///		"lastName":"Andrew",
		///		"emailId":"Mike.Adenuga"
		/// }
		/// </remarks>
		/// <param name="employee"></param>
		///<response code="201"> Returns the new created item </response>
		///<response code="400"> If the item is null</response>

		[HttpPost]
		[Produces("application/json")]
		public Employee Post([FromBody] Employee employee)
		{
			return new Employee();
		}

		[HttpPut("{id}")]
		public void Put(int id, [FromBody] Employee employee)
		{
			//Logic to update an Employee
		}

		private List<Employee> Getemployees()
		{
			return new List<Employee>()
			{ 
				new Employee()
				{
					Id = 1,
					FirstName = "John",
					LastName="Smith",
					EmailId="John.Smith@gmail.com"
				},
				new Employee()
				{
					Id = 2,
					FirstName = "John",
					LastName="Joe",
					EmailId="John.Joe@gmail.com"
				},
				new Employee()
				{
					Id = 3,
					FirstName = "John",
					LastName="Smith",
					EmailId="John.Smith@gmail.com"
				},
				new Employee()
				{
					Id = 4,
					FirstName = "John",
					LastName="Joe",
					EmailId="John.Joe@gmail.com"
				}

			};
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertionsDemo
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
    
    public class PersonAssertions : ReferenceTypeAssertions<Person, PersonAssertions> 
    {
        public PersonAssertions(Person instance) : base(instance) 
        {
            
        }

        protected override string Identifier => "Person";
    }

    public static class PersonAssertionsExtensions
    {
        public static PersonAssertions Should(this Person person)
        {
            return new PersonAssertions(person);
        }

        //public static AndConstraint<PersonAssertions> HaveValidFirstName(this PersonAssertions assertions)
        //{
        //    var firstName = assertions.Subject.FirstName;
        //    Execute.Assertion
        //        .ForCondition(!string.IsNullOrEmpty(firstName)&&firstName.Length >= 2)
        //        .FailWith("Expected");
        //}
    }
}

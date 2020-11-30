using course_sense_dotnet.AlertSystem;
using course_sense_dotnet.Utility;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace course_sense_dotnet_tests
{
    public class ContactValidationTests
    {
        private readonly IContactValidation contactValidation;
        private readonly Mock<ITwilioClientWrapper> twilioClientMock;
        public ContactValidationTests()
        {
            twilioClientMock = new Mock<ITwilioClientWrapper>();
            contactValidation = new ContactValidation(new Mock<ILogger<ContactValidation>>().Object, twilioClientMock.Object);
        }
    }
}

using course_sense_dotnet.NotificationManager.SMSClient;
using course_sense_dotnet.Validators;
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
        private readonly IContactValidator contactValidation;
        private readonly Mock<ISMSClient> twilioClientMock;
        public ContactValidationTests()
        {
            twilioClientMock = new Mock<ISMSClient>();
            contactValidation = new ContactValidator(new Mock<ILogger<ContactValidator>>().Object, twilioClientMock.Object);
        }
    }
}

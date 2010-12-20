using System;
using NUnit.Framework;
using NHamcrest;
using It = NHamcrest.Core;

namespace xray.tests
{
	[TestFixture]
	public class SystemTestingExploratoryTest
	{
		[Test]
		public void sample_syntax()
		{
            var prober = new PollingProber(1000, 100);
			Assert.True(prober.check(Take.Snapshot(() => new {A = 5, B = 6}).
                Has(x => x.B, It.Is.EqualTo(6)).
                Has(x => x.A, It.Is.EqualTo(5))
                ));
		}
	}
}


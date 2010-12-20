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
			Assert.True(Take.Snapshot(() => 6).Has(x => x, It.Is.EqualTo(6)).probeAndMatch());
		}
	}
}


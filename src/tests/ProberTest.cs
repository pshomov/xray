using System;
using System.Threading;
	
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
	
using Rhino.Mocks;

namespace xray.tests
{
	[TestFixture]
	public class ProberTest
	{
		PollingProber prober = new PollingProber(1000,1);

		[Test]
		public void should_check_true_when_probe_matches()
		{
			var probe = MockRepository.GenerateMock<Probe>();
			probe.Stub(x => x.probeAndMatch()).Return(true);
			
			Assert.That(prober.check(probe));
		}
		
		[Test]
		public void should_be_probing_periodically_until_probe_matches()
		{
			var probe = MockRepository.GenerateMock<Probe>();
			probe.Expect(x => x.probeAndMatch()).Return(false).Repeat.Times(2);
			probe.Expect(x => x.probeAndMatch()).Return(true);

			Assert.That(prober.check(probe));
			probe.AssertWasCalled(x => x.probeAndMatch(), opts => opts.Repeat.Times(3));
		}
		
		[Test]
		public void should_be_probing_periodically_while_probe_does_not_match_until_expiry_period()
		{
			var probe = MockRepository.GenerateMock<Probe>();
			probe.Expect(x => x.probeAndMatch()).Return(false);
			Assert.That(!prober.check(probe));
		}
		
		[Test]
		public void should_check_when_all_probes_match()
		{
			var probe1 = MockRepository.GenerateMock<Probe>();
			var probe2 = MockRepository.GenerateMock<Probe>();
			probe1.Expect(x => x.probeAndMatch()).Return(true);
			probe2.Expect(x => x.probeAndMatch()).Return(true);
			
			Assert.That(prober.check(probe1, probe2));
			
			probe1.AssertWasCalled(x => x.probeAndMatch());
			probe2.AssertWasCalled(x => x.probeAndMatch());
		}

		[Test]
		public void should_fail_when_at_least_one_probe_does_not_match()
		{
			var probe1 = MockRepository.GenerateMock<Probe>();
			var probe2 = MockRepository.GenerateMock<Probe>();
			probe1.Expect(x => x.probeAndMatch()).Return(true);
			probe2.Expect(x => x.probeAndMatch()).Return(false);
			
			Assert.That(!prober.check(probe1, probe2));
		}
	}
}


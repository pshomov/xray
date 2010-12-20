using System;
using NHamcrest;
using System.Collections.Generic;
using NHamcrest.Core;

namespace xray
{
    internal class QueryMatcher<T, V> : IMatcher<T>
    {
        private readonly IMatcher<V> _matcher;
        private readonly NHamcrest.Func<T, V> _query;

        public QueryMatcher(NHamcrest.Func<T, V> query, IMatcher<V> matcher)
        {
            _query = query;
            _matcher = matcher;
        }

        public void DescribeTo(IDescription description)
        {
            description.AppendText(_query.ToString()+" matches ");
			_matcher.DescribeTo(description);
        }

        public bool Matches(T item)
        {
            return _matcher.Matches(_query(item));
        }

        public void DescribeMismatch(T item, IDescription mismatchDescription)
        {
            mismatchDescription.AppendText(_query.ToString()+" with parameter "+item.ToString()+"does not match matches ");
			_matcher.DescribeTo(mismatchDescription);
        }
    }
	
    public interface SnapshotExpectationsBuilder<T> : Probe
    {
        void AddMatcher<V>(NHamcrest.Func<T, V> query, IMatcher<V> matcher);
    }

    public static class SnapshotHelpers
    {
        public static SnapshotExpectationsBuilder<T> Has<T, V>(this SnapshotExpectationsBuilder<T> f,
                                                               NHamcrest.Func<T, V> query, IMatcher<V> matcher)
        {
            f.AddMatcher(query, matcher);
            return f;
        }
    }

    public class SnapshotMatchingProbe<T> : SnapshotExpectationsBuilder<T>
    {
        private readonly List<IMatcher<T>> _matcher = new List<IMatcher<T>>();
        private readonly Func<T> _snapshotTaker;

        public SnapshotMatchingProbe(Func<T> snapshotTaker)
        {
            _snapshotTaker = snapshotTaker;
        }

        private IMatcher<T> Matcher
        {
            get { return Matches.AllOf(_matcher); }
        }

        public void AddMatcher<V>(NHamcrest.Func<T, V> query, IMatcher<V> matcher)
        {
            _matcher.Add(new QueryMatcher<T, V>(query, matcher));
        }

        public bool probeAndMatch()
        {
            return Matcher.Matches(_snapshotTaker());
        }
    }
	
	public class Take {
        public static SnapshotExpectationsBuilder<T> Snapshot<T>(Func<T> snapshotTaker)
        {
            return new SnapshotMatchingProbe<T>(snapshotTaker);
        }
	}
}


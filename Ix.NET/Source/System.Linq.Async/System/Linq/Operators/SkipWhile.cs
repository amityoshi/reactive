﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if HAS_ASYNC_ENUMERABLE_CANCELLATION
            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, [System.Runtime.CompilerServices.EnumeratorCancellation]CancellationToken cancellationToken = default)
#else
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
#endif
            {
                await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);

                while (await e.MoveNextAsync())
                {
                    var element = e.Current;

                    if (!predicate(element))
                    {
                        yield return element;

                        while (await e.MoveNextAsync())
                        {
                            yield return e.Current;
                        }

                        yield break;
                    }
                }
            }
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if HAS_ASYNC_ENUMERABLE_CANCELLATION
            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate, [System.Runtime.CompilerServices.EnumeratorCancellation]CancellationToken cancellationToken = default)
#else
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
#endif
            {
                await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);
                var index = -1;

                while (await e.MoveNextAsync())
                {
                    checked
                    {
                        index++;
                    }

                    var element = e.Current;

                    if (!predicate(element, index))
                    {
                        yield return element;

                        while (await e.MoveNextAsync())
                        {
                            yield return e.Current;
                        }

                        yield break;
                    }
                }
            }
        }

        internal static IAsyncEnumerable<TSource> SkipWhileAwaitCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if HAS_ASYNC_ENUMERABLE_CANCELLATION
            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, [System.Runtime.CompilerServices.EnumeratorCancellation]CancellationToken cancellationToken = default)
#else
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
#endif
            {
                await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);

                while (await e.MoveNextAsync())
                {
                    var element = e.Current;

                    if (!await predicate(element).ConfigureAwait(false))
                    {
                        yield return element;

                        while (await e.MoveNextAsync())
                        {
                            yield return e.Current;
                        }

                        yield break;
                    }
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static IAsyncEnumerable<TSource> SkipWhileAwaitWithCancellationCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if HAS_ASYNC_ENUMERABLE_CANCELLATION
            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, [System.Runtime.CompilerServices.EnumeratorCancellation]CancellationToken cancellationToken = default)
#else
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
#endif
            {
                await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);

                while (await e.MoveNextAsync())
                {
                    var element = e.Current;

                    if (!await predicate(element, cancellationToken).ConfigureAwait(false))
                    {
                        yield return element;

                        while (await e.MoveNextAsync())
                        {
                            yield return e.Current;
                        }

                        yield break;
                    }
                }
            }
        }
#endif

        internal static IAsyncEnumerable<TSource> SkipWhileAwaitCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if HAS_ASYNC_ENUMERABLE_CANCELLATION
            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate, [System.Runtime.CompilerServices.EnumeratorCancellation]CancellationToken cancellationToken = default)
#else
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
#endif
            {
                await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);
                var index = -1;

                while (await e.MoveNextAsync())
                {
                    checked
                    {
                        index++;
                    }

                    var element = e.Current;

                    if (!await predicate(element, index).ConfigureAwait(false))
                    {
                        yield return element;

                        while (await e.MoveNextAsync())
                        {
                            yield return e.Current;
                        }

                        yield break;
                    }
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static IAsyncEnumerable<TSource> SkipWhileAwaitWithCancellationCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if HAS_ASYNC_ENUMERABLE_CANCELLATION
            return Core(source, predicate);

            static async IAsyncEnumerable<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate, [System.Runtime.CompilerServices.EnumeratorCancellation]CancellationToken cancellationToken = default)
#else
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
#endif
            {
                await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);
                var index = -1;

                while (await e.MoveNextAsync())
                {
                    checked
                    {
                        index++;
                    }

                    var element = e.Current;

                    if (!await predicate(element, index, cancellationToken).ConfigureAwait(false))
                    {
                        yield return element;

                        while (await e.MoveNextAsync())
                        {
                            yield return e.Current;
                        }

                        yield break;
                    }
                }
            }
        }
#endif
    }
}

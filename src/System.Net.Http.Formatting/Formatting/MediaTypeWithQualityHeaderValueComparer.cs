﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http.Headers;

namespace System.Net.Http.Formatting
{
    internal class MediaTypeWithQualityHeaderValueComparer : IComparer<MediaTypeWithQualityHeaderValue>
    {
        private static readonly MediaTypeWithQualityHeaderValueComparer _mediaTypeComparer = new MediaTypeWithQualityHeaderValueComparer();

        private MediaTypeWithQualityHeaderValueComparer()
        {
        }

        public static MediaTypeWithQualityHeaderValueComparer QualityComparer
        {
            get { return _mediaTypeComparer; }
        }

        public int Compare(MediaTypeWithQualityHeaderValue mediaType1, MediaTypeWithQualityHeaderValue mediaType2)
        {
            Contract.Assert(mediaType1 != null, "The 'mediaType1' parameter should not be null.");
            Contract.Assert(mediaType2 != null, "The 'mediaType2' parameter should not be null.");

            ParsedMediaTypeHeaderValue parsedMediaType1 = new ParsedMediaTypeHeaderValue(mediaType1);
            ParsedMediaTypeHeaderValue parsedMediaType2 = new ParsedMediaTypeHeaderValue(mediaType2);

            int returnValue = CompareBasedOnQualityFactor(parsedMediaType1, parsedMediaType2);

            if (returnValue == 0)
            {
                if (!String.Equals(parsedMediaType1.Type, parsedMediaType2.Type, StringComparison.OrdinalIgnoreCase))
                {
                    if (parsedMediaType1.IsAllMediaRange)
                    {
                        return -1;
                    }
                    else if (parsedMediaType2.IsAllMediaRange)
                    {
                        return 1;
                    }
                }
                else if (!String.Equals(parsedMediaType1.SubType, parsedMediaType2.SubType, StringComparison.OrdinalIgnoreCase))
                {
                    if (parsedMediaType1.IsSubTypeMediaRange)
                    {
                        return -1;
                    }
                    else if (parsedMediaType2.IsSubTypeMediaRange)
                    {
                        return 1;
                    }
                }
            }

            return returnValue;
        }

        private static int CompareBasedOnQualityFactor(ParsedMediaTypeHeaderValue parsedMediaType1, ParsedMediaTypeHeaderValue parsedMediaType2)
        {
            Contract.Assert(parsedMediaType1 != null, "The 'parsedMediaType1' parameter should not be null.");
            Contract.Assert(parsedMediaType2 != null, "The 'parsedMediaType2' parameter should not be null.");

            double qualityDifference = parsedMediaType1.QualityFactor - parsedMediaType2.QualityFactor;
            if (qualityDifference < 0)
            {
                return -1;
            }
            else if (qualityDifference > 0)
            {
                return 1;
            }

            return 0;
        }
    }
}
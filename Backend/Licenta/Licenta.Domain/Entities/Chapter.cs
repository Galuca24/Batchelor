using Licenta.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Domain.Entities
{
    public class Chapter
    {
        public Guid ChapterId { get; set; }
        public string ChapterName { get; set; }
        public string AudioUrl { get; set; }
        public string Duration { get; set; }
        public Guid AudioBookId { get; set; }
        public AudioBook AudioBook { get; set; }

        private Chapter()
        {

        }

        public Chapter(Guid chapterId, string chapterName, string audioUrl, string duration, Guid audioBookId)
        {
            ChapterId = chapterId;
            ChapterName = chapterName;
            AudioUrl = audioUrl;
            Duration = duration;
            AudioBookId = audioBookId;
        }

        public static Result<Chapter> Create(string chapterName, string audioUrl, string duration, Guid audioBookId)
        {
            if (string.IsNullOrWhiteSpace(chapterName))
                return Result<Chapter>.Failure("ChapterName cannot be empty.");

            if (string.IsNullOrWhiteSpace(audioUrl))
                return Result<Chapter>.Failure("AudioUrl cannot be empty.");

            if (string.IsNullOrWhiteSpace(duration))
                return Result<Chapter>.Failure("Duration cannot be empty.");

            if (audioBookId == Guid.Empty)
                return Result<Chapter>.Failure("AudioBookId cannot be empty.");

            var chapter = new Chapter(Guid.NewGuid(), chapterName, audioUrl, duration, audioBookId);

            return Result<Chapter>.Success(chapter);
        }
    }
}

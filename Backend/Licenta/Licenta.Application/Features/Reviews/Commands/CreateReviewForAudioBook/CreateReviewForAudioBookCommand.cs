using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Commands.CreateReviewForAudioBook
{
    public class CreateReviewForAudioBookCommand : IRequest<CreateReviewForAudioBookCommandResponse>
    {
        public Guid UserId { get; set; }
        public Guid AudioBookId { get; set; }
        public string ReviewText { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}

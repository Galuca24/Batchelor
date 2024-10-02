using Licenta.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.AudioBook.Commands.DeleteAudioBook
{
    public class DeleteAudioBookCommand : IRequest<DeleteAudioBookCommandResponse>
    {
        public Guid AudioBookId { get; set; }
    }
    
}

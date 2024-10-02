using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.AudioBook.Commands.UpdateAudioBook
{
    public class UpdateAudioBookCommand : IRequest<AudioBookDto>
    {
        public Guid AudioBookId { get; set; }
        public UpdateAudioBookDto AudioBook { get; set; }
    }
    
}

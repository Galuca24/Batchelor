using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.AudioBook.Commands.CreateAudioBook
{
    public class CreateAudioBookCommandResponse : BaseResponse
    {
        public CreateAudioBookCommandResponse() : base()
        {
        }

        public CreateAudioBookDto AudioBook { get; set; }
    }
    
}

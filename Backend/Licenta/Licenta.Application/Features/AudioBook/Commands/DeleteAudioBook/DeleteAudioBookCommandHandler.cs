using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.AudioBook.Commands.DeleteAudioBook
{
    public class DeleteAudioBookCommandHandler : IRequestHandler<DeleteAudioBookCommand, DeleteAudioBookCommandResponse>
    {
        private readonly IAudioBookRepository _audioBookRepository;

        public DeleteAudioBookCommandHandler(IAudioBookRepository audioBookRepository)
        {
            _audioBookRepository = audioBookRepository;
        }

        public async Task<DeleteAudioBookCommandResponse> Handle(DeleteAudioBookCommand request, CancellationToken cancellationToken)
        {
            var audioBook = await _audioBookRepository.FindByIdAsync(request.AudioBookId);

            if (audioBook == null)
            {
                return new DeleteAudioBookCommandResponse
                {
                    Success = false,
                    Message = "Audiobook-ul nu a fost găsit."
                };
            }
            var result = await _audioBookRepository.DeleteAsync(request.AudioBookId);
            if(!result.IsSuccess)
            {
                return new DeleteAudioBookCommandResponse
                {
                    Success = false,
                    Message = result.Error
                };
            }

            return new DeleteAudioBookCommandResponse
            {
                Success = true,
                Message = "Audiobook-ul a fost șters cu succes."
            };

        }
    }
}

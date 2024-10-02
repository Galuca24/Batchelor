using Ergo.Api.Controllers;
using Licenta.API.Models;
using Licenta.API.Models.AwsS3;
using Licenta.API.Services;
using Licenta.Application.Features.UserPhotos.Command.AddUserPhoto;
using Licenta.Application.Features.UserPhotos.Command.UpdateUserPhoto;
using Licenta.Application.Features.UserPhotos.Queries.GetUserPhoto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudController : ApiControllerBase
    {
        private readonly IStorageService storageService;
        private string AwsKeyEnv { get; set; }
        private string AwsSecretKeyEnv { get; set; }

        public CloudController(IStorageService storageService)
        {

            this.storageService = storageService;
            DotNetEnv.Env.Load();
            AwsKeyEnv = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY")!;
            AwsSecretKeyEnv = Environment.GetEnvironmentVariable("AWS_SECRET_KEY")!;

        }
        [HttpPost]
        [Route("upload")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                await using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var fileExt = Path.GetExtension(file.FileName);
                var objectName = $"{Guid.NewGuid()}{fileExt}";

                var s3Object = new S3Object()
                {
                    BucketName = "galabucket",
                    InputStream = memoryStream,
                    Name = objectName
                };

                var credentials = new AWSCredential()
                {
                    AwsKey = "",
                    AwsSecretKey = ""
                };

                var result = await storageService.UploadFileAsync(s3Object, credentials);

                if (result.StatusCode == 200)
                {
                    return Ok(new { Message = result.Message, FileName = objectName });
                }
                else
                {
                    return StatusCode(result.StatusCode, result.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPost]
        [Route("upload-user-photo")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddUserPhoto(AddUserPhotoDto addUserPhotoDto)
        {
            await using var memoryStr = new MemoryStream();
            await addUserPhotoDto.File.CopyToAsync(memoryStr);
            var fileExt = Path.GetExtension(addUserPhotoDto.File.FileName);
            var objName = $"{Guid.NewGuid()}{fileExt}";
            var s3Object = new S3Object()
            {
                BucketName = "galabucket",
                InputStream = memoryStr,
                Name = objName
            };
            var cred = new AWSCredential()
            {
                AwsKey = AwsKeyEnv,
                AwsSecretKey = AwsSecretKeyEnv
            };
            var result = await storageService.UploadFileAsync(s3Object, cred);
            var command = new AddUserPhotoCommand()
            {
                UserId = addUserPhotoDto.UserId,
                PhotoUrl = objName
            };
            var userPhotoResult = await Mediator.Send(command);
            if (!userPhotoResult.Success)
            {
                await storageService.DeleteFileAsync(objName, cred);
                return BadRequest(userPhotoResult);
            }
            return Ok(userPhotoResult);

        }




        [HttpGet]
        [Route("get-user-photo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserPhoto(string userId)
        {
            var command = new GetUserPhotoQuery()
            {
                UserId = userId
            };
            var userPhotoResult = await Mediator.Send(command);
            if (!userPhotoResult.Success)
            {
                return BadRequest(userPhotoResult);
            }
            return Ok(userPhotoResult);
        }




        [HttpPut]
        [Route("update-user-photo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserPhoto(UpdateUserPhotoDto updateUserPhotoDto)
        {
            await using var memoryStr = new MemoryStream();
            await updateUserPhotoDto.File.CopyToAsync(memoryStr);
            var fileExt = Path.GetExtension(updateUserPhotoDto.File.FileName);
            var objName = $"{Guid.NewGuid()}{fileExt}";
            var s3Object = new S3Object()
            {
                BucketName = "galabucket",
                InputStream = memoryStr,
                Name = objName
            };
            var cred = new AWSCredential()
            {
                AwsKey = AwsKeyEnv,
                AwsSecretKey = AwsSecretKeyEnv
            };

            var result = await storageService.UploadFileAsync(s3Object, cred);
            var command = new UpdateUserPhotoCommand()
            {
                UserPhotoId = updateUserPhotoDto.UserPhotoId,
                PhotoUrl = objName
            };
            var userPhotoResult = await Mediator.Send(command);
            if (!userPhotoResult.Success)
            {
                await storageService.DeleteFileAsync(objName, cred);
                return BadRequest(userPhotoResult);
            }
            var deleteOldPhoto = await storageService.DeleteFileAsync(updateUserPhotoDto.CloudUrl, cred);
            if (!deleteOldPhoto)
            {
                return BadRequest(deleteOldPhoto);
            }
            return Ok(userPhotoResult);
        }

    }


}


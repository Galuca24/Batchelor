using Infrastructure.Repositories;
using Infrastructure;
using Licenta.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Infrastructure.Services;
using Licenta.Application.Contracts;
using Licenta.Application.Contracts.Interfaces;

namespace Licenta.Infrastructure
{
    public static class InfrastructureRegistrationDI
    {
        public static IServiceCollection AddInfrastructureToDI(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<LicentaContext>(
                options => options.UseNpgsql(
                    configuration.GetConnectionString("LicentaConnection"),
                    b => b.MigrationsAssembly(typeof(LicentaContext).Assembly.FullName)
                ));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IPasswordResetCode, PasswordResetCodeRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserPhotoRepository, UserPhotoRepository>();
            services.AddScoped<ICheckoutRepository, CheckoutRepository>();
            services.AddScoped<IFineRepository, FineRepository>();
            services.AddScoped<IFavouriteRepository, FavouriteRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IInboxItemRepository, InboxItemRepository>();
            services.AddScoped<IOpenAIService, OpenAIService>();
            services.AddScoped<IAudioBookRepository, AudioBookRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IChapterRepository, ChapterRepository>();





            return services;
        }
    }
}

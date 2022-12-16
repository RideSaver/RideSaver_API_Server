using DataAccess.DataModels;

namespace IdentityService.Registry
{
    public class AuthorizationRegistry
    {
        public static List<AuthorizationModel> InitializeUserAuthorizationRegistry(Guid userId)
        {
            var AuthorizationList = new List<AuthorizationModel>()
            {
                new AuthorizationModel  // UberBLACK
                {
                    Id = new Guid(),
                    UserId= userId,
                    ServiceId = new Guid("d4abaae7-f4d6-4152-91cc-77523e8165a4"),
                    RefreshhToken= "uber-black-refresh-access-token",
                },

                new AuthorizationModel // UberPOOL
                {
                    Id = new Guid(),
                    UserId= userId,
                    ServiceId = new Guid("26546650-e557-4a7b-86e7-6a3942445247"),
                    RefreshhToken= "uber-pool-refresh-access-token",
                },

                new AuthorizationModel // UberX
                {
                    Id = new Guid(),
                    UserId= userId,
                    ServiceId = new Guid("2d1d002b-d4d0-4411-98e1-673b244878b2"),
                    RefreshhToken= "uber-x-refresh-access-token",
                },

                new AuthorizationModel // Lyft
                {
                    Id = new Guid(),
                    UserId= userId,
                    ServiceId = new Guid("2B2225AD-9D0E-45E0-85FB-378FE2B521E0"),
                    RefreshhToken= "lyft-standard-refresh-access-token",
                },

                new AuthorizationModel // LyftSHARED
                {
                    Id = new Guid(),
                    UserId= userId,
                    ServiceId = new Guid("52648E86-B617-44FD-B753-295D5CE9D9DC"),
                    RefreshhToken= "lyft-shared-refresh-access-token",
                },

                new AuthorizationModel // LyftXL
                {
                    Id = new Guid(),
                    UserId= userId,
                    ServiceId = new Guid("BB331ADE-E379-4F12-9AB0-A68AF94D5813"),
                    RefreshhToken= "lyft-xl-refresh-access-token",
                },

                new AuthorizationModel // LyftLUX
                {
                    Id = new Guid(),
                    UserId= userId,
                    ServiceId = new Guid("B47A0993-DE35-4F86-8DD8-C6462F16F5E8"),
                    RefreshhToken= "lyft-lux-refresh-access-token",
                },
            };

            return AuthorizationList;
        }
    }
}

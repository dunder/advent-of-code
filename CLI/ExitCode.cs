using System.Net;

namespace CLI
{
    /// <summary>
    /// BSD/Unix known exit codes
    /// </summary>
    public enum ExitCode : int
    {
        Ok = 0, // success
        Usage = 64, // EX_USAGE
        NoInput = 66, // EX_NOINPUT  (cookie file missing)
        Unavailable = 69, // EX_UNAVAILABLE (404)
        Software = 70, // EX_SOFTWARE  (internal error)
        OSErr = 71, // EX_OSERR     (network/IO)
        TempFail = 75, // EX_TEMPFAIL  (500 / retry later)
    }

    public enum ErrorMode
    {
        CookieNotFound
    }


    public static class Exit
    {
        public static int Success = (int)ExitCode.Ok;
        // Generic failure
        public static int Fail = (int)ExitCode.Software;

        public static int FromErrorMode(ErrorMode mode) => mode switch
        {
            ErrorMode.CookieNotFound => (int)ExitCode.NoInput,
            _ => (int)ExitCode.Software
        };

        public static int FromHttpStatus(HttpStatusCode code) => code switch
        {
            HttpStatusCode.NotFound => (int)ExitCode.Unavailable,
            HttpStatusCode.InternalServerError => (int)ExitCode.TempFail,
            _ => (int)ExitCode.Software
        };
    }

}

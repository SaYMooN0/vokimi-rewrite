namespace SharedKernel.Common.constants
{
    public class ImgOperationsConsts
    {
        public const string
            CommonFolder = "common",

            ProfilePicturesFolder = "profile_pictures",

            TestConclusionsFolder = "test_conclusions";
        //tests folders
        public const string
            PublishedTestsFolderName = "tests",
            DraftTestsFolderName = "draft_tests";
        //tests related sub folders
        public const string
           AnswersSubFolderName = "answers",
           QuestionsSubFolderName = "questions",
           ResultsSubFolderName = "results";
        //base file names
        public const string
            ProfilePic = "profile_pic",
            TestCoverFileName = "cover",
            QuestionImgFileName = "q_img";
        public static string DefaultTestCoverImg => $"{CommonFolder}/test_cover_default.webp";
        public static string DefaultTestResultImg => $"{CommonFolder}/result.webp";

        public static string DefaultProfilePicture => $"{CommonFolder}/default_profile_picture.webp";
        public static string AnonymousProfilePicture => $"{CommonFolder}/anonymous_profile_picture.webp";

        public static string ImgUrl(string fileKey) =>
           $"vokimiimgs/{fileKey}";
        public static string ImageUrlWithVersion(string path) =>
            $"{ImgUrl(path)}?v={Guid.NewGuid()}";
        public const int MaxImageSizeInBytes = 3 * 1024 * 1024;
        public const int MaxImageSizeInMB = 3;
    }
}

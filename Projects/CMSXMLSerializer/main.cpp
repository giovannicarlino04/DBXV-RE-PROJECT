#include "CMS.hpp"

int main(int argc, char** argv) {
    if (argc != 3) {
        std::cout << "Usage: " << argv[0] << " <input_file> <output_file>" << std::endl;
        return 1;
    }

    std::string inputFileName = argv[1];
    std::string outputFileName = argv[2];

    CMS cms;

    // Check file extension to determine operation
    if (inputFileName.substr(inputFileName.find_last_of(".") + 1) == "cms") {
        // Load CMS data
        cms.Load(inputFileName);

        // Serialize to XML
        cms.SerializeToXML(outputFileName);
        std::cout << "CMS data serialized to XML: " << outputFileName << std::endl;
    } else if (inputFileName.substr(inputFileName.find_last_of(".") + 1) == "xml") {
        // Deserialize XML data
        cms.DeserializeFromXML(inputFileName);

        // Save CMS data
        cms.Save();
        std::cout << "XML data deserialized to CMS: " << outputFileName << std::endl;
    } else {
        std::cout << "Unsupported file format. Please use .cms or .xml files." << std::endl;
        return 1;
    }

    return 0;
}

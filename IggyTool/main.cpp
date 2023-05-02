#include <iostream>
#include <fstream>
#include <cstring>

using namespace std;

int main(int argc, char* argv[]) {
    if (argc < 2) {
        cerr << "Usage: " << argv[0] << " <input file>" << endl;
        return 1;
    }

    // Apri il file di input
    ifstream in(argv[1], ios::binary);
    if (!in.is_open()) {
        cerr << "Unable to open input file" << endl;
        return 1;
    }

    // Trova la posizione del primo blob
    char blob_header[] = {0x46, 0x57, 0x53}; // Header SWF: FWS in ASCII
    int header_size = sizeof(blob_header);

    char buffer[header_size];
    bool found_first_blob = false;

    while (in.read(buffer, header_size)) {
        if (memcmp(buffer, blob_header, header_size) == 0) {
            if (found_first_blob) {
                // Trovato il secondo blob, esci dal ciclo
                break;
            }
            found_first_blob = true;
        }
        // Sposta la posizione di lettura indietro
        in.seekg(-header_size + 1, ios::cur);
    }

    if (!found_first_blob) {
        cerr << "Unable to find first blob in input file" << endl;
        return 1;
    }

    // Salva il primo blob in un nuovo file
    ofstream out("output1.swf", ios::binary);
    if (!out.is_open()) {
        cerr << "Unable to create output file" << endl;
        return 1;
    }

    out.write(buffer, header_size);

    while (in.read(buffer, sizeof(buffer))) {
        if (memcmp(buffer, blob_header, header_size) == 0) {
            // Trovato il secondo header SWF, esci dal ciclo
            break;
        }
        out.write(buffer, sizeof(buffer));
    }

    // Chiudi il primo file di output
    out.close();

    if (in.eof()) {
        // Non è stato trovato il secondo blob
        cerr << "Unable to find second blob in input file" << endl;
        return 1;
    }

    // Salva il secondo blob in un nuovo file
    ofstream out2("output2.swf", ios::binary);
    if (!out2.is_open()) {
        cerr << "Unable to create output file" << endl;
        return 1;
    }

    out2.write(buffer, header_size);

    while (in.read(buffer, sizeof(buffer))) {
        out2.write(buffer, sizeof(buffer));
    }

    // Chiudi il secondo file di output e il file di input
    out2.close();
    in.close();

    cout << "Blobs extracted to output1.swf";
}
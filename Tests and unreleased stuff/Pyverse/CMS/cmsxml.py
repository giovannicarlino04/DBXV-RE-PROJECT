import struct
import xml.etree.ElementTree as ET
import xml.dom.minidom
import CMS


class Parser:
    def __init__(self, location: str, write_xml: bool = False):
        self.save_location = location
        self.raw_bytes = open(location, "rb").read()
        self.bytes = list(self.raw_bytes)
        self.cms_file = CMS.CMS_File()

    def parse(self):
        count = struct.unpack("i", self.raw_bytes[8:12])[0]
        offset = struct.unpack("i", self.raw_bytes[12:16])[0]
        self.cms_file.CMS_Entries = []

        #print(f"Debug: Parsing {count} entries...")  # Debug message

        for i in range(count):
            #print(f"Debug: Parsing entry {i+1}...")  # Debug message

            entry = CMS.CMS_Entry()
            entry.Index = str(struct.unpack("i", self.raw_bytes[offset:offset+4])[0])
            entry.Str_04 = self.get_string(self.raw_bytes, offset+4)
            entry.I_08 = struct.unpack("q", self.raw_bytes[offset+8:offset+16])[0]
            entry.I_16 = struct.unpack("i", self.raw_bytes[offset+16:offset+20])[0]
            entry.I_20 = struct.unpack("H", self.raw_bytes[offset+20:offset+22])[0]
            entry.I_22 = struct.unpack("H", self.raw_bytes[offset+22:offset+24])[0]
            entry.I_24 = struct.unpack("H", self.raw_bytes[offset+24:offset+26])[0]
            entry.I_26 = struct.unpack("H", self.raw_bytes[offset+26:offset+28])[0]
            entry.I_28 = struct.unpack("i", self.raw_bytes[offset+28:offset+32])[0]
            entry.Str_32 = self.get_string(self.raw_bytes, struct.unpack("i", self.raw_bytes[offset+32:offset+36])[0])
            entry.Str_36 = self.get_string(self.raw_bytes, struct.unpack("i", self.raw_bytes[offset+36:offset+40])[0])
            entry.Str_44 = self.get_string(self.raw_bytes, struct.unpack("i", self.raw_bytes[offset+44:offset+48])[0])
            entry.Str_48 = self.get_string(self.raw_bytes, struct.unpack("i", self.raw_bytes[offset+48:offset+52])[0])
            entry.Str_56 = self.get_string(self.raw_bytes, struct.unpack("i", self.raw_bytes[offset+56:offset+60])[0])
            entry.Str_60 = self.get_string(self.raw_bytes, struct.unpack("i", self.raw_bytes[offset+60:offset+64])[0])
            entry.Str_64 = self.get_string(self.raw_bytes, struct.unpack("i", self.raw_bytes[offset+64:offset+68])[0])
            entry.Str_68 = self.get_string(self.raw_bytes, struct.unpack("i", self.raw_bytes[offset+68:offset+72])[0])

            self.cms_file.CMS_Entries.append(entry)
            offset += 80

        #print("Debug: Parsing complete.")  # Debug message



    def get_string(self, data: bytes, offset: int) -> str:
        end_offset = offset
        while end_offset < len(data) and data[end_offset] != 0:
            end_offset += 1
        return data[offset:end_offset].decode("utf-8", errors="ignore")

    def save_to_xml(self, path):
        root = ET.Element("CMS")

        for entry in self.cms_file.CMS_Entries:
            entry_element = ET.SubElement(root, "Entry")
            entry_element.set("ID", entry.Index)

            str_04_element = ET.SubElement(entry_element, "Str_04")
            str_04_element.text = entry.Str_04
            #print(f"Debug: Str_04 = {entry.Str_04}")

            i_08_element = ET.SubElement(entry_element, "I_08")
            i_08_element.text = str(entry.I_08)
            #print(f"Debug: I_08 = {entry.I_08}")

            i_16_element = ET.SubElement(entry_element, "I_16")
            i_16_element.text = str(entry.I_16)
            #print(f"Debug: I_16 = {entry.I_16}")

            i_20_element = ET.SubElement(entry_element, "I_20")
            i_20_element.text = str(entry.I_20)
            #print(f"Debug: I_20 = {entry.I_20}")

            i_22_element = ET.SubElement(entry_element, "I_22")
            i_22_element.text = str(entry.I_22)
            #print(f"Debug: I_22 = {entry.I_22}")

            i_24_element = ET.SubElement(entry_element, "I_24")
            i_24_element.text = str(entry.I_24)
            #print(f"Debug: I_24 = {entry.I_24}")

            i_26_element = ET.SubElement(entry_element, "I_26")
            i_26_element.text = str(entry.I_26)
            #print(f"Debug: I_26 = {entry.I_26}")

            i_28_element = ET.SubElement(entry_element, "I_28")
            i_28_element.text = str(entry.I_28)
            #print(f"Debug: I_28 = {entry.I_28}")

            str_32_element = ET.SubElement(entry_element, "Str_32")
            str_32_element.text = entry.Str_32
            #print(f"Debug: Str_32 = {entry.Str_32}")

            str_36_element = ET.SubElement(entry_element, "Str_36")
            str_36_element.text = entry.Str_36
            #print(f"Debug: Str_36 = {entry.Str_36}")

            str_44_element = ET.SubElement(entry_element, "Str_44")
            str_44_element.text = entry.Str_44
            #print(f"Debug: Str_44 = {entry.Str_44}")

            str_48_element = ET.SubElement(entry_element, "Str_48")
            str_48_element.text = entry.Str_48
            #print(f"Debug: Str_48 = {entry.Str_48}")

            str_56_element = ET.SubElement(entry_element, "Str_56")
            str_56_element.text = entry.Str_56
            #print(f"Debug: Str_56 = {entry.Str_56}")

            str_60_element = ET.SubElement(entry_element, "Str_60")
            str_60_element.text = entry.Str_60
            #print(f"Debug: Str_60 = {entry.Str_60}")

            str_64_element = ET.SubElement(entry_element, "Str_64")
            str_64_element.text = entry.Str_64
            #print(f"Debug: Str_64 = {entry.Str_64}")

            str_68_element = ET.SubElement(entry_element, "Str_68")
            str_68_element.text = entry.Str_68
            #print(f"Debug: Str_68 = {entry.Str_68}")

        xml_string = ET.tostring(root, encoding="utf-8", xml_declaration=True)
        #print(xml_string)  # Debug: Stampa il contenuto XML generato

        with open(path, "wb") as file:
            file.write(xml_string)


# Esempio di utilizzo:
parser = Parser("char_model_spec.cms", write_xml=True)
parser.parse()  # Aggiungi questa riga per chiamare il metodo parse()
parser.save_to_xml("char_model_spec.xml")


#####  ORGANIZZAZIONE DEL FILE XML  #######


def convert_to_new_structure(xml_path):
    # Parse the original XML file
    tree = ET.parse(xml_path)
    root = tree.getroot()

    # Cerca e rimuovi tutte le occorrenze di '#CMS' nel testo dei nodi
    for element in root.iter():
        if element.text and '#CMS' in element.text:
            element.text = element.text.replace('#CMS', '')


    # Save the modified XML to a new file
    tree.write('char_model_spec.xml', encoding='utf-8', xml_declaration=True)

# Specify the path to your original XML file
xml_path = 'char_model_spec.xml'

# Convert the XML to the new structure
convert_to_new_structure(xml_path)

# Carica il file CMS
tree = ET.parse('char_model_spec.xml')
root = tree.getroot()

# Crea il nuovo elemento radice
new_root = ET.Element('CMS')

# Itera su tutti gli elementi Entry nel file CMS originale
for entry in root.findall('Entry'):
    # Ottieni l'attributo ID
    entry_id = entry.get('ID')

    # Crea un nuovo elemento Entry nel nuovo file XML
    new_entry = ET.SubElement(new_root, 'Entry')
    new_entry.set('ID', entry_id)

    # Ottieni il valore di ShortName dall'elemento Str_04
    short_name = entry.find('Str_04').text

    # Aggiungi l'attributo ShortName all'elemento Entry
    new_entry.set('ShortName', short_name)

    # Itera su tutti gli elementi figlio di Entry nel file CMS originale
    for child in entry:
        # Ottieni il nome dell'elemento
        tag_name = child.tag

        # Ottieni il valore dell'elemento
        tag_value = child.text

        # Crea un nuovo elemento nel nuovo file XML
        new_element = ET.SubElement(new_entry, tag_name)

        # Verifica se il valore è None e assegna una stringa vuota al posto di None
        if tag_value is None:
            tag_value = ""

        # Aggiungi l'attributo value all'elemento
        new_element.set('value', tag_value)

# Crea un oggetto file per la scrittura del nuovo file XML
with open('char_model_spec.xml', 'wb') as file:
    # Crea un oggetto ElementTree dal nuovo elemento radice
    new_tree = ET.ElementTree(new_root)

    # Scrivi il contenuto nel file
    new_tree.write(file, encoding='utf-8', xml_declaration=True)


# Carica il file XML
xml_path = 'char_model_spec.xml'
dom = xml.dom.minidom.parse(xml_path)

# Crea una stringa con l'indentazione per formattare il documento XML
xml_str = dom.toprettyxml(indent="  ")

# Scrivi la stringa formattata in un nuovo file XML
new_xml_path = 'char_model_spec.xml'
with open(new_xml_path, 'w', encoding='utf-8') as f:
    f.write(xml_str)
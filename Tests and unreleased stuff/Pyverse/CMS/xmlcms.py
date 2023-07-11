import struct
import xml.etree.ElementTree as ET
import CMS


class Deserializer:
    def __init__(self, xml_path):
        self.xml_path = xml_path
        self.cms_file = CMS.CMS_File()

    def deserialize(self):
        tree = ET.parse(self.xml_path)
        root = tree.getroot()

        for entry in root.findall('Entry'):
            cms_entry = CMS.CMS_Entry()
            cms_entry.Index = entry.get('ID')
            cms_entry.Str_04 = entry.get('ShortName')

            for child in entry:
                tag_name = child.tag
                tag_value = child.get('value')

                if tag_name == 'I_08':
                    cms_entry.I_08 = int(tag_value)
                elif tag_name == 'I_16':
                    cms_entry.I_16 = int(tag_value)
                elif tag_name == 'I_20':
                    cms_entry.I_20 = int(tag_value)
                elif tag_name == 'I_22':
                    cms_entry.I_22 = int(tag_value)
                elif tag_name == 'I_24':
                    cms_entry.I_24 = int(tag_value)
                elif tag_name == 'I_26':
                    cms_entry.I_26 = int(tag_value)
                elif tag_name == 'I_28':
                    cms_entry.I_28 = int(tag_value)
                elif tag_name.startswith('Str_'):
                    setattr(cms_entry, tag_name, tag_value)

            self.cms_file.CMS_Entries.append(cms_entry)

    def save_to_cms(self, path):
        self.cms_file.sort_entries()
        self.cms_file.save_binary(path)


# Esempio di utilizzo:
deserializer = Deserializer('char_model_spec.xml')
deserializer.deserialize()
deserializer.save_to_cms('char_model_spec.cms')

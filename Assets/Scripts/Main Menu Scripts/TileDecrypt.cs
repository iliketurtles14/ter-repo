using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;

public class TileDecrypt : MonoBehaviour
{
    public void DecryptTileset(string path)
    {
        string pythonScript = @"#! /usr/bin/env python3

# escapistscrypto
# Copyright © 2021 Przemysław Piasecki
# check the end of this file for license information

from pathlib import Path
from hashlib import md5
from configparser import ConfigParser
import argparse
import blowfish

def decrypt(ifile, ofile, strip_null = True, key = b""mothking""):
    ifile.seek(0,0)
    ciphertext = ifile.read()

    cipher = blowfish.Cipher(key, byte_order = ""little"")
    plaintext = b"""".join(cipher.decrypt_ecb(ciphertext))

    if strip_null:
        plaintext = plaintext.rstrip(b""\0"")

    ofile.write(plaintext)

def encrypt(ifile, ofile, key = b""mothking""):
    #pad with \0 to a size multiple of 8 bytes
    ifile.seek(0,2)
    size = ifile.tell()
    ifile.seek(0,0)

    plaintext = ifile.read() + bytes((8 - size) % 8)

    cipher = blowfish.Cipher(key, byte_order = ""little"")
    ciphertext = b"""".join(cipher.encrypt_ecb(plaintext))

    ofile.write(ciphertext)

def hash(path):
    hashString = ""l0l_"" + str(path.stat().st_size)
    return md5(hashString.encode(""UTF8"")).hexdigest()

def create_config_parser(file):
    #data files contain unmarked comments before the first section
    #that need to be skipped before passing it to a ConfigParser object
    while True:
        pos = file.tell()
        line = file.readline()
        if not line:
            raise EOFError(""The file doesn't appear to be valid"")
        line = line.lstrip()
        if line and line[0] == ""["":
            break
    file.seek(pos)

    parser = ConfigParser()
    parser.read_file(file)

    return parser

def make_valid(orig_val_file, validate_dir, force):
    if not validate_dir.is_dir():
        raise NotADirectoryError

    languages = [""eng"",""fre"",""ger"",""spa"",""rus"",""pol"",""ita""]
    files = [""data"",""items"",""speech""]
    hashlookup = {""data"":0,""items"":1,""speech"":2}

    hashes = create_config_parser(orig_val_file)

    for lang in languages:
        langhashes = hashes[""Val""][lang[0]].split(""_"")

        for file in files:
            filepath = validate_dir.joinpath(file + ""_"" + lang + "".dat"")
            if filepath.is_file():
                langhashes[hashlookup[file]] = hash(filepath)

        hashes[""Val""][lang[0]] = ""_"".join(langhashes)

    if force:
        mode = ""w""
    else:
        mode = ""x""

    with validate_dir.joinpath(""val.dat"").open(mode,encoding=""UTF-16"") as file:
        hashes.write(file,space_around_delimiters=False)

def open_ifile(ipath):
    try:
        ifile = ipath.open(""rb"")
    except FileNotFoundError:
        errmsg = ipath.name + "" doesn't exist""
        raise ValueError(errmsg)
    return ifile

def open_ofile(opath,force):
    if force:
        ofile = opath.open(""wb"")
    else:
        try:
            ofile = opath.open(""xb"")
        except FileExistsError:
            errmsg = opath.name + "" already exists! use -f to overwrite it""
            raise ValueError(errmsg)
    return ofile

def cli_dec(input, output = None, force = False, keep_null = False):
    ipath = Path(input)

    if output:
        opath = Path(output)
    else:
        opath = ipath.with_stem(ipath.stem + ""_decr"")

    decrypt(open_ifile(ipath),open_ofile(opath,force), not keep_null)

def cli_enc(input, output = None, force = False):
    ipath = Path(input)

    if output:
        opath = Path(output)
    else:
        opath = ipath.with_stem(ipath.stem + ""_encr"")

    encrypt(open_ifile(ipath),open_ofile(opath,force))

def cli_val(path , val_base, force = False):
    valpath = Path(val_base)
    dirpath = Path(path)

    try:
        valfile = valpath.open(""r"",encoding=""UTF-16"")
    except FileNotFoundError:
        errmsg = valpath.name + "" doesn't exist""
        raise ValueError(errmsg)

    try:
        make_valid(valfile, dirpath, force)
    except FileExistsError:
        errmsg = ""val.dat file already exists in the directory! use -f to overwrite it""
        raise ValueError(errmsg)

def cli():
    parser = argparse.ArgumentParser(description=""Decrypt or encrypt escapists files"")

    fileio = argparse.ArgumentParser(add_help=False)

    fileio.add_argument(""input"", help=""specify input file"")
    fileio.add_argument(""-o"", ""--output"", help=""specify output file"")
    fileio.add_argument(""-f"", ""--force"", help=""will overwrite any existing output file"", action='store_true')

    modes = parser.add_subparsers()

    decrypt  = modes.add_parser(""dec"", help=""decrypt file"", parents=[fileio])
    encrypt  = modes.add_parser(""enc"", help=""encrypt file"", parents=[fileio])
    validate = modes.add_parser(""val"", help=""create a valid val.dat for modified text files"")

    decrypt.add_argument(""-n"", ""--keep-null"", help=""keep tailing null bytes in a decrypted file"", action='store_true')
    decrypt.set_defaults(func=cli_dec)

    encrypt.set_defaults(func=cli_enc)

    validate.add_argument(""path"", help=""specify path to directory with text files"")
    validate.add_argument(""val_base"", help=""specify path to an original val.dat file"")
    validate.add_argument(""-f"", ""--force"", help=""will overwrite any existing val.dat file"", action='store_true')
    validate.set_defaults(func=cli_val)

    args = vars(parser.parse_args())

    if not args:
        parser.error(""no arguments"")

    func = args.pop(""func"")

    try:
        func(**args)
    except ValueError as exc:
        parser.error(exc.args[0])

if __name__ == ""__main__"":
    cli()
    
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program. If not, see <http://www.gnu.org/licenses/>.
";

        string tempScriptPath = Path.GetTempFileName() + ".py";
        File.WriteAllText(tempScriptPath, pythonScript);

        var psi = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = $"\"{tempScriptPath}\" dec \"{path}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = Process.Start(psi))
        {
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            process.WaitForExit();
        }
    }
}
